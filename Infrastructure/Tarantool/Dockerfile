FROM centos:7

EXPOSE 8081
EXPOSE 3301
EXPOSE 3302
EXPOSE 8082
EXPOSE 8084

RUN yum install -y git \
                   cmake \
                   make \
                   gcc \
                   gcc-c++ \
                   unzip

RUN curl -L https://tarantool.io/nBkMdwS/release/2/installer.sh | bash
RUN yum install -y tarantool tarantool-devel
RUN yum install -y cartridge-cli
RUN yum install -y telnet

WORKDIR /app
RUN cartridge create --name test && cd test

WORKDIR /app/test
RUN sed -ie "/app.roles.custom/a 'app.roles.test'," init.lua
RUN sed -ie "/app.roles.custom/a 'cartridge.roles.crud-storage'," init.lua
RUN sed -ie "/app.roles.custom/a 'cartridge.roles.crud-router'," init.lua
RUN sed -ie "/tarantool/a 'crud == 0.12.0'," test-scm-1.rockspec
RUN sed -ie "/tarantool/a 'ddl == 1.6.2'," test-scm-1.rockspec
RUN sed -ie "/tarantool/a 'metrics == 0.3.0-1'," test-scm-1.rockspec

RUN rm /app/test/instances.yml
RUN rm /app/test/replicasets.yml

COPY test.lua /app/test/app/roles/
COPY instances.yml /app/test/
COPY replicasets.yml /app/test/
COPY schema.yml /app/test/

RUN cartridge build

ENTRYPOINT cartridge start -d && \
    sleep 3 && \
    (cartridge replicasets setup --bootstrap-vshard || true) && \
    cartridge log -f