

if google cred is not setup then error will be 
```
The Application Default Credentials are not available. They are available if running in Google Compute Engine. Otherwise, the environment variable GOOGLE_APPLICATION_CREDENTIALS must be defined pointing to a file defining the credentials. See https://developers.google.com/accounts/docs/application-default-credentials for more information.'
```

### args:
To view table/test bigquery
./BigQueryPublicTableForIds 1

To create table with entries from input.csv

./BigQueryPublicTableForIds 2 input.csv OptionalTablePostFix