window.API = {
    async post(url, data) {
        const res = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": APP_CONFIG.token
            },
            body: JSON.stringify(data)
        });

        if (!res.ok) throw new Error(await res.text());
        return res.json();
    }
};