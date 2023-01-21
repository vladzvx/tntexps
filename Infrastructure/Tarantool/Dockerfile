FROM centos:7

EXPOSE 8081
EXPOSE 3301
EXPOSE 3302

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
RUN cartridge create --name my_test_cluster && cd my_test_cluster

WORKDIR /app/my_test_cluster
RUN sed -ie "/app.roles.custom/a 'app.roles.my_test_cluster'," init.lua
RUN sed -ie "/app.roles.custom/a 'cartridge.roles.crud-storage'," init.lua
RUN sed -ie "/app.roles.custom/a 'cartridge.roles.crud-router'," init.lua
RUN sed -ie "/tarantool/a 'crud == 0.14.1'," my_test_cluster-scm-1.rockspec
RUN sed -ie "/tarantool/a 'ddl == 1.6.2'," my_test_cluster-scm-1.rockspec
#RUN sed -ie "/tarantool/a 'metrics == 0.3.0-1'," my_test_cluster-scm-1.rockspec

RUN rm /app/my_test_cluster/instances.yml
RUN rm /app/my_test_cluster/replicasets.yml

COPY my_test_cluster.lua /app/my_test_cluster/app/roles/
COPY instances.yml /app/my_test_cluster/
COPY replicasets.yml /app/my_test_cluster/
COPY schema.yml /app/my_test_cluster/

RUN cartridge build

ENTRYPOINT cartridge start -d && \
    sleep 3 && \
    (cartridge replicasets setup --bootstrap-vshard || true) && \
    cartridge log -f