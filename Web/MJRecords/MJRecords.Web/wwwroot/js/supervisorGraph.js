google.charts.load('current', { packages: ['corechart'] });

google.charts.setOnLoadCallback(drawChart);

function drawChart() {
    var dataArray = [
        ['Month', 'Monthly Expenses ($)', { role: 'annotation' }, 'PO Count', { role: 'annotation' }]
    ];

    for (let i = 0; i < monthsJson.length; i++) {
        dataArray.push([
            monthsJson[i],
            totalsJson[i],
            '$' + totalsJson[i].toFixed(2),   
            countsJson[i],
            countsJson[i] + ' POS'
        ]);
    }

    var data = google.visualization.arrayToDataTable(dataArray);

    var container = document.getElementById('supervisorGraph');

    var options = {
        title: 'Monthly Expenses and PO Count',
        vAxes: {
            0: { title: 'Amount ($)' },
            1: { title: 'PO Count' },
        },
        hAxis: { title: 'Month' },
        seriesType: 'bars',
        series: {
            0: { targetAxisIndex: 0, color: '#3672e9' },
            1: { targetAxisIndex: 1, color: '#ff6384' },
        },
        annotations: {
            alwaysOutside: true,
            textStyle: {
                fontSize: 12,
                color: '#000',
                auraColor: 'none',
                bold: true,
            },
            boxStyle: {
                stroke: '#888',
                strokeWidth: 1,
                rx: 4,
                ry: 4,
                fill: '#fff',
                padding: 4,
            },
            stem: {
                length: 0,
                color: '#000'
            }
        },
        legend: { position: 'top' },
        bar: { groupWidth: '60%' },
        width: container.offsetWidth,
        height: container.offsetHeight,
    };

    var formatter = new google.visualization.NumberFormat({
        prefix: '$',
        groupingSymbol: ',',
        fractionDigits: 2
    });
    formatter.format(data, 2);
    var chart = new google.visualization.ComboChart(container);
    chart.draw(data, options);
}

window.addEventListener('resize', drawChart);
