SERVICE_ROOT=/home/ec2-user/Hourglass

# Filename for log file based on current date
FILENAME=$(date "+%Y-%m-%d_%H.%M.%S")_hourglass.log
LOG_FILE=/home/ec2-user/ServerLogs/$FILENAME

# Create the log file
sudo touch $LOG_FILE
sudo chmod 755 $LOG_FILE

# Start the ASP.NET core application
cd $SERVICE_ROOT/build_output

# Different server configuration files are used for the dev/staging server and the production/release server
# these are the application names specified in CodeDeploy
if [ "$APPLICATION_NAME" == "reach-be-deploy-dev" ]
then
    nohup /usr/local/bin/dotnet --environment "Development" HourglassServer.dll &>$LOG_FILE &
fi

if [ "$APPLICATION_NAME" == "reach-be-deploy-prod" ]
then
    nohup /usr/local/bin/dotnet --environment "Production" HourglassServer.dll &>$LOG_FILE &
fi


