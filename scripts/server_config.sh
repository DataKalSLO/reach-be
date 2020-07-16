# Copy over Apache server configuration

# Different server configuration files are used for the dev/staging server and the production/release server
# these are the application names specified in CodeDeploy
if [ "$APPLICATION_NAME" == "reach-be-deploy-dev" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_staging.conf /etc/httpd/conf.d/server.conf
    # Tells server to use the development database for the staging website, instead of the prod database.
    # This is a hacky workaround but I cant figure out the right way.
    sudo mv /home/ec2-user/Hourglass/build_output/appsettings.Development.json /home/ec2-user/Hourglass/build_output/appsettings.Production.json
fi

if [ "$APPLICATION_NAME" == "reach-be-deploy-prod" ]
then
    sudo cp /home/ec2-user/Hourglass/scripts/server_production.conf /etc/httpd/conf.d/server.conf
fi

# Start the Apache service
sudo systemctl start httpd.service
