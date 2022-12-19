### How to install gitlab in Docker

#### 1. Pull the images from DockerHub

`docker pull gitlab/gitlab-ce`

#### 2. Run the container in docker

- Create the volumes for container

```bash
mkdir /etc/gitlab/config/ /data/gitlab/logs/ /data/gitlab/data/
```

- run the container

```bash
docker run -d -p 443:443 -p 80:80 -p 222:22  \
--name ByGitlab   \
--restart always  \
-v /etc/gitlab/config:/etc/gitlab  \
-v /data/gitlab/logs:/var/log/gitlab  \
-v /data/gitlab/data:/var/opt/gitlab  \
gitlab/gitlab-ce
```



#### 3. Install the gitlab/gitlab-runner and register that

- pull the image


```bash
docker pull docker.io/gitlab/gitlab-ce
```

- run the container


```bash
docker run --rm -itd --name gitlabrunner -v /data/devops/gitlab:/etc/gitlab-runner gitlab/gitlab-runner
```

- register the runner
  - interactive way in container

  ```bash
  #execute the container
  docker exec -it gitlabrunner bash
  #run the command
  gitlab-runner register
  #Enter the GitLab instance URL (for example, https://gitlab.com/):
  https://gitlab.com
  #Enter the registration token:
  some token of your gitlab
  #Enter a description for the runner:
  some descripetion
  #Enter tags for the runner (comma-separated):
  some tags for jobs to run
  #Enter the executor,custom, docker, shell,ssh,kubernetes,docker-ssh,parallels,virtualbox
  detail executor name
  #successfully
  ```

  - non-interactive

  ```bash
  #still need to execute the container
  gitlab-runner register \
  --non-interactive      \
  --executor "shell"     \
  --url "https://gitlab.com"   \
  --registration-token "some token of your gitlab" \
  --description "some descripetion"   \
  --tag-list "build,deploy"   \
  --run-untagged="true"  \
  --locked="false"   \
  --access-level="not_protected"
  ```

  [Please check docs for the detail of executor](https://docs.gitlab.com/runner/executors/#executors)