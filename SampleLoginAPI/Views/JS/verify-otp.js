document.getElementById("verifyForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const payload = {
        otp: document.getElementById("otp").value
    };

    try {
        const response = await fetch("/api/account/verify-otp", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        const result = await response.json();
        document.getElementById("message").innerText = result.message;

        if (response.ok) {
            document.getElementById("message").style.color = "green";
        } else {
            document.getElementById("message").style.color = "red";
        }
    } catch (err) {
        document.getElementById("message").innerText = "Error for verifying OTP.";
    }
});
