window.chartBalance = function (xaxis, data) {

    var ctx = document.getElementById('balanceChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: xaxis,
            datasets: [{
                data: data,
                backgroundColor: [ 'rgba(255, 99, 132, 0.2)' ],
                borderColor: [ 'rgba(255, 99, 132, 1)' ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            legend: { display: false },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}