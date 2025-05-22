document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const payload = {
        emailOrPhone: document.getElementById("emailOrPhone").value,
        password: document.getElementById("password").value
    };

    try {
        const response = await fetch("/api/account/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        const result = await response.json();
        document.getElementById("message").innerText = result.message;

        if (response.ok) {
            setTimeout(() => {
                window.location.href = "/views/verify-otp.html";
            }, 1000);
        }
    } catch (err) {
        document.getElementById("message").innerText = "Error for sending OTP.";
    }
});
