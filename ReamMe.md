Designet to run on Raspbian.

Building project for linux:
dotnet publish -c Release -r linux-arm

The best way to run this program on boot is to use cron.

To open cron table use:
sudo crontab -e

then at the botton add line:
@reboot /home/pi/NetRadioPlayer/NetRadioPlayer.Device
assuming that files are in "/home/pi/NetRadioPlayer/" folder 
and NetRadioPlayer.Device has executable permissions (chmod +x).