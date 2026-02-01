import WebSocket from 'ws';

const url = 'wss://ggj-2026-production-8567.up.railway.app';

console.log(`Connecting to ${url}...`);

const ws = new WebSocket(url);

ws.on('open', () => {
    console.log('✅ Connected successfully!');
    
    // Send a test message (simple string first)
    ws.send('test message');
    
    setTimeout(() => {
        ws.close();
    }, 2000);
});

ws.on('message', (data) => {
    console.log('Received:', data.toString());
});

ws.on('error', (error) => {
    console.error('❌ WebSocket error:', error.message);
});

ws.on('close', (code, reason) => {
    console.log(`🔌 Connection closed. Code: ${code}, Reason: ${reason || 'No reason'}`);
});
