rem run once as administrator to allow kinect12Scratch to set up a server and accept connections from other pc's

rem allow program to open a port (absolutely necessary):
netsh http add urlacl url=8000 user=Everyone listen=yes

rem allow program to accept connections from other pcs (not strictly necessary):
netsh advfirewall firewall add rule name="opening for Kinect12Scratch" dir=in action=allow localip=any localport=8000 protocol=tcp