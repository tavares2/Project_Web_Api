@echo off
echo Starting LocalStack...
docker-compose up -d

echo Waiting for LocalStack to initialize...
timeout 10

echo Setting up DynamoDB table...
powershell -ExecutionPolicy Bypass -File setup-dynamodb.ps1

echo LocalStack is ready.
