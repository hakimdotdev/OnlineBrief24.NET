
# CitizensOfficeAppointments
[![Docker Image CI](https://github.com/hakimdotdev/OnlineBrief24UI/actions/workflows/docker-image.yml/badge.svg)](https://github.com/hakimdotdev/OnlineBrief24UI/actions/workflows/docker-image.yml)

.NET Application with Docker support as a UI for OnlineBrief24.

## Notes 

- Supposed to be ran as Docker container
- LDAP-Authentication
- Cookie Store
- Using SQL-Server (Entity Framework)
- Uploading to OnlineBrief24-API with possibility of storing data locally

## Docker

### Configuration

All configuration is provided by environment variables.

| Name             | Description                                                | Example              | Default                                         | Required |   |
|------------------|------------------------------------------------------------|----------------------|-------------------------------------------------|----------|---|
| SFTPHOST         | URL of the SFTP Host                                       | my.sftphost.tld      | api.onlinebrief24.de                            | NO       |   |
| SFTPUSER         | SFTP Login Username                                        | first@lastname.tld   |                                                 | YES      |   |
| SFTPPASSWORD     | SFTP Login Passwort                                        | MyS3cureP4$$         |                                                 | YES      |   |
| THUMBPRINT       | MD5 Fingerprint of the Host                                | 1a:2b:3c:4d:5e:6f... | 5b:d1:29:17:cb:5e:17:b9:e2:29:4e:1e:aa:c7:d9:b2 | NO       |   |
| VERIFYTHUMBPRINT | Determines if the SFTP Client verifies the MD5 Fingerprint | true                 | false                                           | NO       |   |
| UPLOADPATH       | Path that files are being uploaded to                      | /my/path             | /upload/api                                     | NO       |   |
| LDAPSERVERADRESS | FQDN of the Domaincontroller / LDAP-Server                 | dc01.my.domain       |                                                 | NO       |   |
|                  |                                                            |                      |                                                 |          |   |
|                  |                                                            |                      |                                                 |          |   |

### Getting Started

```
 docker run -p 39999:39999 -e CATEGORY=x -e CONCERN=x -e EMAILHOST=x -e EMAILUSER=x -e EMAILPASSWORD=x -e EMAIL=x hakimdotnet/citizensofficeappointments:latest