
# PowerShell script to create a DynamoDB table in LocalStack

$awsCommand = 'aws --endpoint-url=http://localhost:4566 dynamodb create-table ' +
    '--table-name ProductTable ' +
    '--attribute-definitions AttributeName=id,AttributeType=S ' +
    '--key-schema AttributeName=id,KeyType=HASH ' +
    '--provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5'

# Execute the AWS CLI command
Invoke-Expression $awsCommand
