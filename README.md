# DockerPOC
this is POC where frontend REST API(WebApplication2) calls the backend API(InvokeExe). 
Backend API invokeExe then uses a local HelloWorld.exe and returns the message returned from HelloWorld.exe
to the frontend app.

Thereafter these apps are containerized via kubernetes. 

1.) Go to Webapplication2 app asp.net project and build is locally - https://github.com/deepika087/DockerPOC/tree/master/WebApplication2 

2.) Create two folders Frontend and Backend locally which will hold the Docker file and kubernetes yaml file + the artifact generated by respective apps.  

3.) Copy the artifacts generated in step 1 to the folder created in Step 2. something like this https://github.com/deepika087/DockerPOC/tree/master/FrontEnd/publish. 

4.) Make sure to keep the name publish itself, otherwise you will have to change the docker file.  

5.) Now, build the HelloWorld project as the generated exe will be used by Backend project. Backend project is InvokeExe.  

6.) Publish the HelloWorld.exe and copy it the following folder, https://github.com/deepika087/DockerPOC/tree/master/Backend/publish/Data 

7.) to build HelloWorld.exe use the following command. (As of now the backend code assumes Kubernetes linux worker node. Ideally the code should be cross platform. As you can see that https://github.com/deepika087/DockerPOC/blob/master/InvokeExe/InvokeExe/Controllers/InvokeExeController.cs file calls "HelloWorld" not "HelloWorld.exe" as that is the way to call exe on linux platform)

dotnet publish -c Release -r linux-x64 --self-contained true 

8.) Now build InvokeExe and copy the artifacts to Backend\publish.  

9.) Build the frontend and backend images locally and apply the kubernetes YAML files respectively.  

cd C:\Poc\FrontEnd 

docker build -t frontend . 

cd C:\Poc\BackEnd 

docker build -t backend . 

cd C:\Poc\FrontEnd 

kubectl apply -f backend.yaml 

cd C:\Poc\FrontEnd 

kubectl apply -f frontend.yaml 

The main challenge in this entire code is how to make kubectl pick the latest image built locally. As imagePullPolicy: never won't help in this case. For this, 
1.) Open powershell.  

2.) From Docker dashboard, switch to Linux containers. 

3.) On powershell, minikube start 

4.) Now go to the folder where you want to build the dockerfile and build it. Say,  

docker build –t backend:v3 . 

      Tip: Update the version so that you are sure that kubernetes containers use the latest image.  

5.) Change the yaml file to pick up backend: v3 and set imagePullPolicy to Never [because we are on our local] 

6.) Now, switch back to docker UI -> kubernetes -> docker-desktop. Now you can switch back to windows containers and apply the yaml file. The latest image should be picked.   

 

10.) Run the following commands to see the pods/deployment/services. 

kubectl get pods 

kubectl get services 

kubectl get deployments 

 

11.) https://github.com/deepika087/DockerPOC/blob/master/InvokeExe/InvokeExe/Controllers/InvokeExeController.csTo login to a specific pod. Copy the pod name from kubectl get pods 

kubectl exec --stdin --tty backend-podName-cv2tj -- /bin/bash 

 

Go to http://localhost:8081/weatherforecast. The "summary" field of weatherforecast data model is returned from the backend.  
To verify the backend only, go to  http://localhost:8080/invokeexe
Tip: although the documentation says that writing imageName:latest and setting imagePolicy to Never will fetch the latest from local but that doesn’t work.  

 
