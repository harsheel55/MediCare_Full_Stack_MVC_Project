<script>
    function showToast(message, isError) {
        const toast = document.getElementById("toastMessage");
    const content = document.getElementById("toastContent");

    toast.classList.remove("hidden", "error");
    content.textContent = message;

    if (isError) {
        toast.classList.add("error");
        }

        // Auto hide after 5 seconds
        setTimeout(() => {
        hideToast();
        }, 5000);
    }

    function hideToast() {
        const toast = document.getElementById("toastMessage");
    toast.classList.add("hidden");
    }

    // Check server messages from TempData
    window.onload = function () {
        @if (TempData["SuccessMessage"] != null)
    {
        <text>
            showToast("@TempData["SuccessMessage"].ToString().Replace("\"", "\\\"")", false);
        </text>
    }

    @if (TempData["ErrorMessage"] != null)
    {
        <text>
            showToast("@TempData["ErrorMessage"].ToString().Replace("\"", "\\\"")", true);
        </text>
    }
    };
</script>
