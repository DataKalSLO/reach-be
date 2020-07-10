# Copy over Apache server configuration
# Different server configuration files are used for the dev/staging server and the production/release server
if [ "$APPLICATION_NAME" == "reach-be-dev" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_staging.conf /etc/httpd/conf.d/server.conf
fi

if [ "$APPLICATION_NAME" == "reach-be-prod" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_production.conf /etc/httpd/conf.d/server.conf
fi

# Start the Apache service
sudo systemctl start httpd.service
