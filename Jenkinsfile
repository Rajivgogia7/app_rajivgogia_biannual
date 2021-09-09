pipeline {
    agent any
		
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet'
		registry = 'rajivgogia/productmanagementapi'
		username = 'rajivgogia'
        appName = 'ProductManagementApi'
        branchName = 'feature'
        port = 7400
   	}	
   
	options {
        //Prepend all console output generated during stages with the time at which the line was emitted.
		timestamps()
		
		//Set a timeout period for the Pipeline run, after which Jenkins should abort the Pipeline
		timeout(time: 1, unit: 'HOURS') 

        //Skip checking out code from source control by default in the agent directive
		skipDefaultCheckout()
		
		buildDiscarder(logRotator(
			// number of build logs to keep
            numToKeepStr:'3',
            // history to keep in days
            daysToKeepStr: '15'
			))
    }
    
    stages {
        
        stage('Checkout from GitHub'){
            steps{
               checkout([   $class: 'GitSCM',
               branches: [[name: '*/feature']],
               userRemoteConfigs: [[credentialsId: 'GitCreds', url: 'https://github.com/Rajivgogia7/app_rajivgogia_biannual']]
               ])
            }
        }

    	stage ("Nuget restore") {
            steps {
                echo "Nuget restore step"
                bat "dotnet restore"
            }
        }
                  
      	stage('Start sonarqube analysis'){
            steps {
				  echo "Start sonarqube analysis step"
                  withSonarQubeEnv('Test_Sonar') {
                   bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll begin /k:sonar-${userName}-bi-annual /n:sonar-${userName}-bi-annual /v:1.0"
                  }
            }
        }

        stage('Code build') {
            steps {
				  //Cleans the output of a project
				  echo "Clean Previous Build"
                  bat "dotnet clean"
				  
				  //Builds the project and all of its dependencies
                  echo "Code Build"
                  bat 'dotnet build -c Release -o "ProductManagementApi/app/build"'	
            }
        }

         stage('Unit test') {
            steps {
                bat 'dotnet test ProductManagementApi-tests\\ProductManagementApi-tests.csproj -l:trx;logFileName=TestResults.xml'
            }
            post {
                always {
                    xunit(
                    thresholds: [skipped(failureThreshold: '0'), failed(failureThreshold: '0')],
                    tools: [MSTest(pattern: 'ProductManagementApi-tests/TestResults/TestResults.xml')]
                    )
                }
            }
        }

      	stage('Stop sonarqube analysis'){
			steps {
				   echo "Stop sonarqube analysis"
                   withSonarQubeEnv('Test_Sonar') {
                   bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll end"
                   }
            }
        }

        stage ("Release artifact") {
            steps {
                echo "Release artifact step"
                bat "dotnet publish -c Release -o ${appName}/app/${userName}"
            }
        }

        stage ("Docker Image") {
            steps {
                echo "Docker Image step"
                bat "docker build -t i-${userName}-${branchName}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
            }
        }

        stage ("Containers") {
            failFast true
            parallel {
                stage ("PrecontainerCheck") {
                    steps {
                        echo "PrecontainerCheck step"
                        script {
						
							env.containerId = bat(script: "docker ps -a -f publish=${port} -q", returnStdout: true).trim().readLines().drop(1).join('')
                            if (env.containerId != '') {
                                echo "Stopping and removing container running on ${port}"
                                bat "docker stop $env.containerId"
                                bat "docker rm $env.containerId"
                            } else {
                                echo "No container running on ${port}  port."
                            }
                        }
                    }
                }

                stage ("PushtoDTR") {
                    steps {
                        echo "PushtoDTR step"
                         bat "docker tag i-${userName}-${branchName}:${BUILD_NUMBER} ${registry}:i-${userName}-${branchName}-${BUILD_NUMBER}"
                         bat "docker tag i-${userName}-${branchName}:${BUILD_NUMBER} ${registry}:i-${userName}-${branchName}-latest"

                        bat "docker push ${registry}:i-${userName}-${branchName}-${BUILD_NUMBER}"
                        bat "docker push ${registry}:i-${userName}-${branchName}-latest"
                    }
                }
            }
        }

        stage ("Deployment") {
            failFast true
            parallel {
                stage ("Docker deployment") {
                    steps {
                        echo "Docker deployment step"
                        bat "docker run --name c-${userName}-${branchName} -d -p ${port}:80 ${registry}:i-${userName}-${branchName}-latest"
                    }
                }

                stage('Kubernetes Deployment') {
                    steps{
                        bat "kubectl apply -f deployment.yaml"
                    }
                }
            }
        }
   	 }

	 post { 
			always { 
				echo 'Workspace Cleanup'
				cleanWs()
			}
	}
}
