CREATE TABLE Chunk (
  Id_metadata integer(10) NOT NULL, 
  Hash        varchar(255) NOT NULL UNIQUE, 
  Position    integer(10) NOT NULL, 
  "Size"      integer(10) NOT NULL, 
  PRIMARY KEY (Id_metadata, 
  Hash), 
  FOREIGN KEY(Id_metadata) REFERENCES Metadata(Id) ON DELETE CASCADE);
CREATE TABLE Metadata (
  Id                INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  "Size"            integer(10) NOT NULL, 
  Creation_date     timestamp DEFAULT CURRENT_TIMESTAMP, 
  Modification_date timestamp DEFAULT CURRENT_TIMESTAMP, 
  Name              varchar(255) NOT NULL);
CREATE TABLE "Replication" (
  Id_server     integer(10) NOT NULL, 
  Hash_chunk    varchar(255) NOT NULL, 
  Id_metadata   integer(10) NOT NULL, 
  Creation_date timestamp DEFAULT CURRENT_TIMESTAMP, 
  PRIMARY KEY (Id_server, 
  Hash_chunk, 
  Id_metadata), 
  FOREIGN KEY(Id_server) REFERENCES Server(Id) ON DELETE CASCADE, 
  FOREIGN KEY(Id_metadata, Hash_chunk) REFERENCES Chunk(Id_metadata, Hash) ON DELETE CASCADE);
CREATE TABLE Server (
  Id       INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  Address  varchar(255) NOT NULL, 
  Memory   integer(10) NOT NULL, 
  Priority integer(10) NOT NULL);
CREATE TABLE Virtual_node (
  Id_server integer(10) NOT NULL, 
  Hash      varchar(255) NOT NULL UNIQUE, 
  PRIMARY KEY (Id_server, 
  Hash), 
  FOREIGN KEY(Id_server) REFERENCES Server(Id) ON DELETE CASCADE);