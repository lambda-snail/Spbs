function createChart(id, config) {
    let ctx = document.getElementById(id)
    try {
        return new Chart(id, config);
    } catch (e)
    {
        console.log(e);
    }
}