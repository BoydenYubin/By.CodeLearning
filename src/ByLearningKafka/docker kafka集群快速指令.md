### docker kafka集群快速设置指令

#### 1、建立文件夹

```bash
mkdir /opt/zookeeper
mkdir /opt/kafka-broker01
mkdir /opt/kafka-broker02
mkdir /opt/kafka-broker03
```

#### 2、建立kafka docker网络

```bash
docker network create kafka-net
docker network inspect kafka-net
```

#### 3、创建docker集群

```bash
#如果有错误，则删除相应容器
docker rm -f kafka-broker03 kafka-broker02 kafka-broker01 zookeeper-master
#删除之前的logs记录
rm -rf /opt/kafka-broker01/logs/  /opt/kafka-broker02/logs/  /opt/kafka-broker03/logs/ 
#启动集群
docker-compose -f /home/kafka-docker-compose.yml up -d
# rm -f /home/kafka-docker-compose.yml
```

#### 4、kafka容器内部客户端命令

```bash
#进入容器
docker exec -it kafka-broker01 /bin/bash
cd /opt/kafka/bin

#topic命令
kafka-topics.sh --bootstrap-server 192.168.0.3:9092 --create --topic by-first-topic --partitions 3 --replication-factor 3 --command-config /opt/kafka/config/sasl.properties

kafka-topics.sh --bootstrap-server 192.168.0.3:9092 --describe --topic by-first-topic --command-config /opt/kafka/config/sasl.properties

kafka-topics.sh --bootstrap-server 192.168.0.3:9092 --list --command-config /opt/kafka/config/sasl.properties

#producer命令
kafka-console-producer.sh --topic by-first-topic --bootstrap-server 192.168.0.3:9092 --producer.config /opt/kafka/config/sasl.properties

#consumer命令
kafka-consumer-groups.sh --bootstrap-server 192.168.0.3:9092 --group test_consumer --topic by-first-topic --reset-offsets --to-offset 0 --execute --command-config /opt/kafka/config/sasl.properties

kafka-consumer-groups.sh --bootstrap-server 192.168.0.3:9092 --group test_consumer --describe --command-config /opt/kafka/config/sasl.properties
```

#### 5、Zookeeper容器内部命令

```bash
docker exec -it zookeeper-master /bin/bash
```