FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /pdf_generator.api



# Copy everything else and build
COPY /pdf_generator.api/* ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0.11-bullseye-slim-amd64

### Install unoconv utility
RUN apt-get update
RUN apt-get install --assume-yes netcat unoconv curl

ARG OO_VERSION=3.2.1
ARG OO_TGZ_URL="http://ftp5.gwdg.de/pub/openoffice/archive/stable/${OO_VERSION}/OOo_${OO_VERSION}_Linux_x86-64_install-deb_en-US.tar.gz"

ENV SOFFICE_DAEMON_PORT=8100
ENV APP_ROOT=/opt/app-root
ENV PATH=${APP_ROOT}/bin:${PATH} HOME=${APP_ROOT}

WORKDIR /
RUN (curl -0 $OO_TGZ_URL | tar -zx -C /tmp)
RUN chmod u+x /tmp/OOO320_m18_native_packed-1_en-US.9502/update
RUN dpkg -i /tmp/OOO320_m18_native_packed-1_en-US.9502/DEBS/*.deb



WORKDIR /pdf_generator.api
COPY --from=build-env /pdf_generator.api/out .
COPY /pdf_generator.api/convert.sh convert.sh
RUN chmod u+x convert.sh