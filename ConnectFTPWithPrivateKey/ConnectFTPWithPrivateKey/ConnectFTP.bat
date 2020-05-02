cd /d "D:\Sample Projects\ConnectFTPWithPrivateKey\ConnectFTPWithPrivateKey\PSFTP"
psftp -l tkm_hawaii2 -2 -i "D:\Sample Projects\ConnectFTPWithPrivateKey\ConnectFTPWithPrivateKey\HA-Sabre_pri.key" 151.193.132.144 -b "D:\Sample Projects\ConnectFTPWithPrivateKey\ConnectFTPWithPrivateKey\FTPCMDS.txt"
