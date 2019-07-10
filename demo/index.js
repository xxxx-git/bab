if(window.fetch) {
    let token;

    (async () => {
        const token = await getToken();
        console.log(token);
        const user = await getUser();
    })();

}

async function getToken() {
    const headers = new Headers({
        'Content-Type': 'application/json',
    });
    const token = await fetch("https://127.0.0.1:5001/api/token/generate", 
    { 
        mode: 'cors', 
        body: JSON.stringify({ id: 1, displayname: 'pdony', hierarchy: 'ynodp' }), 
        method: "POST", 
        headers: headers,
        credentials: 'include',
        
    }).then(res => res.text());

    return token;
}

async function getUser() {
    const headers = new Headers({
    });
    const token = await fetch("https://127.0.0.1:5001/api/token/verify", 
    { 
        mode: 'cors', 
        method: "GET", 
        credentials: 'include'
        
    }).then(res => res.json());

    return token;
}