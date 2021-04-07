pipeline {
	agent any
	
	environment {
		TAG = sh(script: "echo dev-`date +%Y%m%d%H%M`", returnStdout: true).trim()
		Registry = "registry.gitlab.com/rayprosoft/turnkey-gaming-system/"
		gitlab_user_pwd = credentials('gitlab-user-pwd')
		Component = "sms-service"
	}

	stages {
		stage('Clone') {
			steps {
				git branch: "develop", url: 'https://gitlab.com/rayprosoft/sms-service.git' , credentialsId: 'gitlab-user-pwd'
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
				    sh "sudo docker login registry.gitlab.com -u ${gitlab_user_pwd_USR} -p ${gitlab_user_pwd_PSW}"
					sh 'sudo docker build -f SmsService/Dockerfile -t registry.gitlab.com/rayprosoft/turnkey-gaming-system/${Component}:${TAG} . --secret id=nuget,src="/var/lib/jenkins/.nuget/NuGet/NuGet.Config"'
					sh 'sudo docker push registry.gitlab.com/rayprosoft/turnkey-gaming-system/${Component}:${TAG}'
				}
			}
		}
		
		stage('Deployment') {
			steps {
				script {
					withKubeConfig([credentialsId: 'k8s-kube-config', serverUrl: 'https://192.168.50.105:6443']) {
						sh "kubectl set image deployment/${Component} ${Component}=${Registry}${Component}:${TAG} --record -n dev"
					}
				}
			}
		}
		
		stage('Push Nuget') {   
			steps {
				sh  'dotnet pack SmsReceiver/SmsReceiver.csproj'
				sh  'dotnet pack SmsServiceClient/SmsServiceClient.csproj'
				sh  'dotnet nuget push **/TGS*.nupkg --source "raypro" --skip-duplicate'
				sh  'dotnet nuget push **/TGS*.nupkg --source "tgs-nuget" --skip-duplicate'
			}
		}	
	}
}