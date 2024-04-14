#!/bin/bash
set -e

# Start SQL Server
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
# Replace 'sa' and 'YourPassword' with your credentials
while ! /opt/mssql-tools/bin/sqlcmd -S localhost -U $SQL_USER -P $SQL_PASSWORD -Q "SELECT 1" > /dev/null 2>&1; do
    echo "Waiting for SQL Server to start..."
    sleep 1
done

# Run the SQL script
/opt/mssql-tools/bin/sqlcmd -S localhost -U $SQL_USER -P $SQL_PASSWORD -i /usr/src/app/init.sql

# Keep container running
wait
