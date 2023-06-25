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
    clearChart(chart);
    
    chart.data.labels.push(label);
    chart.data.datasets.forEach((dataset) => {
        dataset.data.push(data);
    });
}

function updateChart(chart)
{
    chart.update();
}

function clearChart(chart) {
    chart.data.labels.pop();
    chart.data.datasets.forEach((dataset) => {
        dataset.data.pop();
    });
}

function destroyChart(chart)
{
    chart.destroy();
}