#### 通过Docker建立Seqlog日志服务器(并通过nginx建立https连接)

#### 1、拉取镜像

```bash
docker pull datalust/seq
```

#### 2、设置初始admin(初始用户名)密码

```bash
PH=$(echo 'BySeq@1992' | docker run --rm -i datalust/seq config hash)
```

#### 3、运行seq容器

- 添加容器卷

```bash
mkdir /data/seqlog
```

- 运行容器

```bash
docker run --name seqlog -d --restart unless-stopped    \
-e ACCEPT_EULA=Y                                        \
-e SEQ_FIRSTRUN_ADMINPASSWORDHASH="$PH"                 \
-e SEQ_API_REDIRECTHTTPTOHTTPS=false                    \
-v /data/seqlog:/data                                   \
-p 8000:80 -p 15341:5341                                \
datalust/seq
```

- 其他初始环境变量

[官网具体环境变量参数说明](https://docs.datalust.co/docs/json-config#settings)

```json
{
  "api": {
    "avoidLdap": false,
    "canonicalUri": null,
    "corsAllowedOrigins": [],
    "frameAncestors": [],
    "hardSessionExpirySeconds": null,
    "hstsIncludeSubDomains": false,
    "hstsMaxAge": 31536000,
    "idleSessionExpirySeconds": 172800,
    "ingestionPort": 45341,
    "integratedAuthenticationScheme": null,
    "listenUris": [
      "https://seq.example.com",
      "https://seq.example.com:45341"
    ],
    "redirectHttpToHttps": false,
    "minRequestBodyDataRateBytesPerSecond": 240.0,
    "minRequestBodyDataRateGracePeriodMilliseconds": 5000
  },
  "cache": {
    "largeObjectHeapCompaction": "Default",
    "systemRamTarget": 0.9,
    "activeFilePageDiscount": null
  },
  "certificates": {
    "certificatesPath": null,
    "defaultPassword": null
  },
  "diagnostics": {
    "internalLogPath": null,
    "internalLoggingLevel": "Information",
    "internalLogServerApiKey": null,
    "internalLogServerUrl": null,
    "metricsSamplingIntervalSeconds": 300,
    "nativeStorageMetricsSamplingIntervalMinutes": 60,
    "telemetryServerUrl": null
  },
  "features": {
    "enabled": []
  },
  "firstRun": {
    "adminPasswordHash": null,
    "adminUsername": null,
    "requireAuthenticationForHttpIngestion": false
  },
  "metastore": {
    "msSql": {
      "connectionString": null,
      "msiResource": null,
      "msiTenantId": null,
      "schema": null
    },
    "postgres": {
      "connectionString": null,
      "schema": null
    }
  },
  "secretKey": {
    "provider": null,
    "providerArgs": null
  },
  "services": {
    "servicesBaseUri": null
  },
  "storage": {
    "secretKey": "pmk.fSB5H/AXSU+lJw8wS84p9Q==",
    "flare": {
      "diskReaderLimit": 5,
      "indexerPriority": 0.0,
      "queryParallelism": 4
    },
    "disableFreeDiskSpaceChecking": false,
    "maximumFutureTimestampDriftSeconds": 3420
  }
}
```

#### 4、获取https证书

[获取证书地址](https://freessl.cn/)，选择一年期域名或者5年期域名

<img src="https://freessl.cn/static/images/trustasia_logo.png" height="30%" width="30%" />

#### 5、通过nginx部署https证书

- 拉取nginx镜像

```bash
docker pull nginx
```

- 添加容器卷

```bash
mkdir /data/nginx
```

- 复制nginx参数

  - nginx.conf

  ```nginx
  user  nginx;
  worker_processes  auto;
  
  error_log  /var/log/nginx/error.log notice;
  pid        /var/run/nginx.pid;
  
  
  events {
      worker_connections  1024;
  }
  
  
  http {
      include       /etc/nginx/mime.types;
      default_type  application/octet-stream;
  
      log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
                        '$status $body_bytes_sent "$http_referer" '
                        '"$http_user_agent" "$http_x_forwarded_for"';
  
      access_log  /var/log/nginx/access.log  main;
  
      sendfile        on;
      #tcp_nopush     on;
  
      keepalive_timeout  65;
  
      #gzip  on;
  
      include /etc/nginx/conf.d/*.conf;
  }
  ```

  - nginx\conf.d\detailserver.conf

  ```nginx
  server {
      listen       443 ssl;
      server_name  seq.webui;
  
      #access_log  /var/log/nginx/host.access.log  main;
      index index.html index.htm;
      
      #为虚拟主机指定pem格式的证书文件
      ssl_certificate      cert/seq.bylearning.cn_chain.crt;
      #为虚拟主机指定私钥文件
      ssl_certificate_key  cert/seq.bylearning.cn_key.key;
      
      location / {
  	    proxy_pass   http://localhost:8000;
          index index.html index.htm;
      }
  
      #ssl_session_cache   shared:SSL:10m;
      ssl_session_timeout  5m;
      #指定使用的ssl协议
   
      ssl_protocols TLSv1 TLSv1.1 TLSv1.2;
   
      #ssl密钥套件
      ssl_ciphers ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-ECDSA-CHACHA20-POLY1305:ECDHE-RSA-CHACHA20-POLY1305:DHE-RSA-AES128-GCM-SHA256:DHE-RSA-AES256-GCM-SHA384;
      ssl_prefer_server_ciphers  on;
      #error_page  404              /404.html;
  
      # redirect server error pages to the static page /50x.html
      # error_page   500 502 503 504  /50x.html;
  }
  server {
  	listen       5341 ssl;
      server_name  seq.ingest;
     
  	index index.html index.htm;
     
      #为虚拟主机指定pem格式的证书文件
      ssl_certificate      cert/bylearning.seqlog.cert.pem;
      #为虚拟主机指定私钥文件
      ssl_certificate_key  cert/bylearning.seqlog.key.pem;
   
      #ssl_session_cache   shared:SSL:10m;
      ssl_session_timeout  5m;
      #指定使用的ssl协议
   
      ssl_protocols TLSv1 TLSv1.1 TLSv1.2;
   
      #ssl密钥套件
      ssl_ciphers ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-ECDSA-CHACHA20-POLY1305:ECDHE-RSA-CHACHA20-POLY1305:DHE-RSA-AES128-GCM-SHA256:DHE-RSA-AES256-GCM-SHA384;
      ssl_prefer_server_ciphers  on;
      location / {
          proxy_pass   http://localhost:15341;
      }
  }
  ```

  

#### 6、运行nginx容器

```shell
docker run --name nginx-seqlog  -p 443:443 -p 5341:5341  -v /data/nginx/html:/usr/share/nginx/html  -v /data/nginx/logs:/var/log/nginx   -v /data/nginx:/etc/nginx  -d nginx
```

