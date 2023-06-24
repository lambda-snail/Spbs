function createChart(id, config) {
    let ctx = document.getElementById(id)
    try {
        return new Chart(id, config);
    } catch (e)
    {
        console.log(e);
    }
}

function addDataToChart(chart, label, data) {
    chart.data.labels.push(label);
    chart.data.datasets.forEach((dataset) => {
        dataset.data.push(data);
    });
    chart.update();
}

function clearChart(chart) {
    chart.data.labels.pop();
    chart.data.datasets.forEach((dataset) => {
        dataset.data.pop();
    });
    chart.update();
}