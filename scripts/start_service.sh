SERVICE_ROOT=/home/ec2-user/Hourglass

# Filename for log file based on current date
FILENAME=$(date "+%Y-%m-%d_%H.%M.%S")_hourglass.log
LOG_FILE=/home/ec2-user/ServerLogs/$FILENAME

# Create the log file
sudo touch $LOG_FILE
sudo chmod 755 $LOG_FILE

# Start the ASP.NET core application
cd $SERVICE_ROOT/build_output
nohup /usr/local/bin/dotnet HourglassServer.dll &>$LOG_FILE &
