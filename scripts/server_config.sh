# Copy over Apache server configuration
# Different server configuration files are used for the staging server and the production server
if [ "$APPLICATION_NAME" == "hourglass-server-staging" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_staging.conf /etc/httpd/conf.d/server.conf
fi

if [ "$APPLICATION_NAME" == "hourglass-server-production" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_production.conf /etc/httpd/conf.d/server.conf
fi

# Start the Apache service
sudo systemctl start httpd.service
