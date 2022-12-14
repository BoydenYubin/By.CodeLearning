#!/usr/bin/env bash

source  /etc/profile
date=$(date)

if [ -z $1 ];then
    echo "请添加下载路径"
    exit
fi

wget -P /tmp $1 -O /tmp/www.zip
if [ $? -ne 0 ] ;then
echo "----------------下载失败---------------" >> /root/update.log
exit
else
echo "----------------下载成功---------------" >> /root/update.log
fi

mkdir /tmp/Webupdate
cd /tmp/Webupdate
unzip /tmp/www.zip

if [ $? -ne 0 ] ;then
echo "-----解压失败-----" >> /root/update.log
rm -rf /tmp/Webupdate
#WebName 是自己定义的用supervisor守护的web服务名称
systemctl status upgradeweb.service
exit
else
echo "-----解压成功-----" >> /root/update.log
#WebName 是自己定义的用supervisor守护的web服务名称
systemctl stop upgradeweb.service
echo "-----停止服务-----" >> /root/update.log
#/usr/local/www/ 是web所在目录
rsync -au /tmp/Webupdate/ /opt/upgradeweb/
echo "-----同步文件成功-----" >> /root/update.log
systemctl start upgradeweb.service
echo "-----重新启动服务-----" >> /root/update.log
systemctl status upgradeweb.service
echo "-----启动服务成功-----" >> /root/update.log
rm -rf /tmp/Webupdate
rm -rf /tmp/www.zip
fi