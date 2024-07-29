kubectl apply -f 01-sql-data-pvc.yaml
kubectl apply -f 02-sql-log-pvc.yaml
kubectl apply -f 03-sql-secrets-pvc.yaml
kubectl apply -f 04-quickfood-sqlserver-deployment.yaml
kubectl apply -f 05-quickfood-sqlserver-service.yaml
kubectl apply -f 06-quickfood-backend-deployment.yaml
kubectl apply -f 07-quickfood-backend-service.yaml