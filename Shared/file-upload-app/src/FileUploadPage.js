import axios from 'axios';
import React, {useEffect, useState} from 'react';

const FileUploadPage = () => {
  const [files, setFiles] = useState([]);
  const [loading, setLoading] = useState(false);

  // Fetch file metadata on component mount
  useEffect(() => {
    fetchFiles();
  }, []);

  const fetchFiles = async () => {
    try {
      setLoading(true);
      const response = await axios.get('http://localhost:5091/api/file');
      setFiles(response.data);
    } catch (error) {
      console.error('Error fetching files', error);
    } finally {
      setLoading(false);
    }
  };

  const handleFileDrop = async (event) => {
    event.preventDefault();
    const droppedFiles = event.dataTransfer.files;

    if (droppedFiles.length) {
      const formData = new FormData();
      formData.append('file', droppedFiles[0]);

      try {
        await axios.post('http://localhost:5091/api/file', formData);
        fetchFiles();  // Refresh the file list
      } catch (error) {
        console.error('Error uploading file', error);
      }
    }
  };

  const handleFileDownload = (fileName) => {
    window.location.href = `http://localhost:5091/api/file/${fileName.id}`;
  };

  const handleFileDelete = async (fileName) => {
    try {
      await axios.delete(`http://localhost:5091/api/file/${fileName.id}`);
      fetchFiles();  // Refresh the file list after deletion
    } catch (error) {
      console.error('Error deleting file', error);
    }
  };

  return (
    <div
      style={
    {
      fontFamily: 'Arial, sans-serif', width: '400px', margin: '50px auto',
          border: '2px dashed #ccc', borderRadius: '10px', padding: '20px',
          textAlign: 'center',
    }}
      onDragOver={(e) => e.preventDefault()}
      onDrop={handleFileDrop}
    >
      <h3 style={{
    margin: '0 0 20px' }}>Загрузите свои файлы сюда</h3>
      {loading ? (
        <p>Loading files...</p>
      ) : (
        <div style={{
    display: 'flex', flexWrap: 'wrap', gap: '10px', justifyContent: 'center' }}>
          {files.map((file) => (
            <div
      key = {file} style = {
        {
          display: 'flex', flexDirection: 'column', alignItems: 'center',
              cursor: 'pointer', position: 'relative',
        }
      } > <
          div
      style =
      {
        {
          width: '50px', height: '50px', backgroundColor: '#f0f0f0',
              border: '1px solid #ddd', borderRadius: '5px', display: 'flex',
              alignItems: 'center', justifyContent: 'center',
              marginBottom: '5px',
        }
      } onClick =
          {() => handleFileDownload(file)} >
                📄 <
              /div>
              <span style={{ fontSize: "12px", textAlign: "center" }}>{file.name}</span><
          button
                style={
    {
      position: 'absolute', top: '0', right: '0', background: 'red',
          color: 'white', border: 'none', borderRadius: '50%', width: '20px',
          height: '20px', cursor: 'pointer',
    }}
                onClick={() => handleFileDelete(file)}
              >
                ×
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

const ServerManagementPage = () => {
  const [servers, setServers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [editingServer, setEditingServer] = useState(null);
  const [newServer, setNewServer] = useState({ address: '', priority: '', memory: '' });

  useEffect(() => {
    fetchServers();
  }, []);

  const fetchServers = async () => {
    try {
      setLoading(true);
      const response = await axios.get('http://localhost:5173/api/server');
      setServers(response.data);
    } catch (error) {
      console.error('Error fetching servers', error);
    } finally {
      setLoading(false);
    }
  };

  const handleServerUpdate = async (serverId, updatedData) => {
    try {
      await axios.put(`http://localhost:5173/api/server/${serverId}`, updatedData);
      fetchServers();
      setEditingServer(null);
    } catch (error) {
      console.error('Error updating server', error);
    }
  };

  const handleServerDelete = async (serverId) => {
    try {
      await axios.delete(`http://localhost:5173/api/server/${serverId}`);
      fetchServers();
    } catch (error) {
      console.error('Error deleting server', error);
    }
  };

  const handleServerAdd = async () => {
    try {
      await axios.post('http://localhost:5173/api/server', newServer);
      fetchServers();
      setNewServer({ address: '', priority: '', memory: '' });
    } catch (error) {
      console.error('Error adding server', error);
    }
  };

  return (
    <div
      style={{
        fontFamily: 'Arial, sans-serif',
        width: '600px',
        margin: '50px auto',
        padding: '20px',
        textAlign: 'center',
        border: '1px solid #ccc',
        borderRadius: '10px',
      }}
    >
      <h3 style={{ margin: '0 0 20px' }}>Server Management</h3>
      {loading ? (
        <p>Loading servers...</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
          {servers.map((server) => (
            <div
              key={server.id}
              style={{
                display: 'flex',
                flexDirection: 'column',
                gap: '10px',
                border: '1px solid #ddd',
                borderRadius: '5px',
                padding: '10px',
              }}
            >
              {editingServer?.id === server.id ? (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '5px' }}>
                  <input
                    type='text'
                    placeholder='Address'
                    value={editingServer.address}
                    onChange={(e) =>
                      setEditingServer({ ...editingServer, address: e.target.value })
                    }
                  />
                  <input
                    type="text"
                    placeholder="Priority"
                    value={editingServer.priority}
                    onChange={(e) =>
                      setEditingServer({ ...editingServer, priority: e.target.value })
                    }
                  />
                  <input
                    type='text'
                    placeholder='Memory'
                    value={editingServer.memory}
                    onChange={(e) =>
                      setEditingServer({ ...editingServer, memory: e.target.value })
                    }
                  />
                  <button
                    onClick={() => handleServerUpdate(server.id, editingServer)}
                  >
                    Save
                  </button>
                  <button onClick={() => setEditingServer(null)}>Cancel</button>
                </div>
              ) : (
                <>
                  <span>Address: {server.address}</span>
                  <span>Priority: {server.priority}</span>
                  <span>Memory: {server.memory}</span>
                  <button onClick={() => setEditingServer(server)}>Edit</button>
                </>
              )}
              <button onClick={() => handleServerDelete(server.id)} style={{ color: "red" }}>
                Delete
              </button>
            </div>
          ))}
        </div>
      )}
      <div style={{ marginTop: '20px' }}>
        <h4>Add New Server</h4>
        <input
          type="text"
          placeholder="Address"
          value={newServer.address}
          onChange={(e) => setNewServer({ ...newServer, address: e.target.value })}
        />
        <input
          type='text'
          placeholder='Priority'
          value={newServer.priority}
          onChange={(e) => setNewServer({ ...newServer, priority: e.target.value })}
        />
        <input
          type="text"
          placeholder="Memory"
          value={newServer.memory}
          onChange={(e) => setNewServer({ ...newServer, memory: e.target.value })}
        />
        <button onClick={handleServerAdd}>Add Server</button>
      </div>
    </div>
  );
};



const App1 =
    () => {
      const [currentPage, setCurrentPage] = useState('files');

  return (
    <div>
      <div
  style =
      {
        {
          display: 'flex', justifyContent: 'center', gap: '20px',
              padding: '10px', backgroundColor: '#f0f0f0',
              borderBottom: '1px solid #ddd',
        }
      } > <button onClick = {() => setCurrentPage('files')} style = {
        {
          padding: '10px 20px', cursor: 'pointer'
        }
      }>Files</button>
        <button onClick={() => setCurrentPage('servers')} style={{ padding: '10px 20px', cursor: 'pointer' }}>
          Servers
        </button>
      </div>

      {currentPage === 'files' && <FileUploadPage />
    } {currentPage === 'servers' && <ServerManagementPage />} <
    /div>
  );
};

export default App1;

