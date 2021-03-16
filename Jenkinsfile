pipeline {
	agent any
	
	environment {
		Registry = "registry.gitlab.com/rayprosoft/sms-system/"
	}
	
	parameters {
		gitParameter branchFilter: 'origin/(.*)', defaultValue: 'develop', name: 'BRANCH', type: 'PT_BRANCH'
	}

	stages {
		stage('Clone') {
			steps {
				git branch: "${params.BRANCH}", url: 'https://gitlab.com/rayprosoft/sms-service.git' , credentialsId: 'gitlab-user-pwd'
			}
		}
	
		stage('Build') {
			steps {
				sh "dotnet build SmsService.sln"
			}
		}
		
		stage('Test') {
			steps {
				sh  "dotnet test SmsService.sln"  
			}
		}
		
		stage('Docker Image') {
			steps {
				script {
					sh 'sudo docker build -f SmsService/Dockerfile --build-arg "BuildConfiguration=Release" -t registry.gitlab.com/rayprosoft/turnkey-gaming-system/sms-service:dev .'
					sh 'sudo docker push registry.gitlab.com/rayprosoft/turnkey-gaming-system/sms-service:dev'
				}
			}
		}
		
		stage('Pre Nuget') {   
			steps {
				timeout(time:1, unit: 'MINUTES') {
				   input message: "Push to nuget ?"
				}
			}
		}
		
		stage('Push Nuget') {   
			steps {
				sh  'dotnet pack SmsReceiver/SmsReceiver.csproj'
				sh  'dotnet pack SmsServiceClient/SmsServiceClient.csproj'
				sh  'dotnet nuget push **/*.nupkg --source "gitlab" --skip-duplicate'
			}
		}	
	}
}