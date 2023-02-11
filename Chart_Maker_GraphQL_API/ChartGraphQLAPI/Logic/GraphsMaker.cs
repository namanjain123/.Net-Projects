using QuickChart;
namespace ChartGraphQLAPI.Logic
{
    public class GraphsMaker
    {
        public string Graph_Maker(DataSource data)
        {
            
            Chart qc = new Chart();
            qc.Width = 500;
            qc.Height = 300;

            var result = "{type: '" + 
                data.type + 
                "',data: {labels: [" 
                + data.label_x + 
                "],datasets: [{label: '"+
                data.label_y +
                "',data: "
                +data.data+
                ",backgroundColor: ["
                +data.colour+
                @"],
            },
        ],
    },
    options: {
layout:{
padding:
{
top:30,
bottom:20
}},
legend:{display:false,},
        scales: {
yAxes: {
ticks:{beginAtZero: true,padding:10}
          },
xAxes: {
ticks:{beginAtZero: true,padding:0}
          },
        },
plugins: {
        datalabels: {
            anchor: 'end',
            align: 'top',
            color:'#53565b',
            font: {
                weight: 'bold'
            }
        }
    }
      },
    }";
            qc.Config = result;
            var sh = qc.GetShortUrl().ToString();
            return sh;
        }
    }
}
