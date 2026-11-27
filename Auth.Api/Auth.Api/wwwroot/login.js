function handleCredentialResponse(response) {
    
    console.log("ID Token de Google recibido:", response.credential);
    document.getElementById("resultado").innerText = "Validando con el servidor...";

    
    fetch('/api/auth/google-login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ idToken: response.credential })
    })
        .then(res => res.json())
        .then(data => {
            document.getElementById("resultado").innerText = JSON.stringify(data, null, 2);
        })
        .catch(err => {
            document.getElementById("resultado").innerText = "Error: " + err;
        });
}