# Stop the apache service
sudo systemctl stop httpd.service

# Stop the dotnet service, if it is running
if pgrep dotnet; then
    sudo killall -9 dotnet;
fi
