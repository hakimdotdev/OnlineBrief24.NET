# OnlineBrief24.NET

.NET Application with Docker support as a UI for OnlineBrief24.

## !Still in progress!
## !Auth currently permits any mail starting with admin, remove for anything but development!
## Notes 

- Upload documents via SFTP
- LDAP-Authentication
- Cookie Sessions

The project is set up to use MSSQL with Entity Framework. If you're not using the LocalDB you might need to add credentials to the connection string. You can use any database that is supported by EF by installing needed packages and adapting the connection string to it.

## Docker

### Configuration

All configuration is provided by environment variables.

| Name             | Description                                                | Default                                         | Required |   |
|------------------|------------------------------------------------------------|-------------------------------------------------|----------|---|
| SFTPHOST         | URL of the SFTP Host                                       | api.onlinebrief24.de                            | NO       |   |
| SFTPUSER         | SFTP Login Username                                        |                                                 | YES      |   |
| SFTPPASSWORD     | SFTP Login Passwort                                        |                                                 | YES      |   |
| THUMBPRINT       | MD5 Fingerprint of the Host                                | 5b:d1:29:17:cb:5e:17:b9:e2:29:4e:1e:aa:c7:d9:b2 | NO       |   |
| VERIFYTHUMBPRINT | Determines if the SFTP Client verifies the MD5 Fingerprint | false                                           | NO       |   |
| UPLOADPATH       | Path that files are being uploaded to                      | /upload/api                                     | NO       |   |
| LDAPSERVERADRESS | FQDN of the Domaincontroller / LDAP-Server                 |                                                 | NO       |   |

### Getting Started
