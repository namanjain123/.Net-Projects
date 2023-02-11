using ChartGraphQLAPI.Logic;
using ChartGraphQLAPI.Models;

namespace ChartGraphQLAPI.Graph
{
    public class Query
    {
        public GraphsMaker gc;
        public Charts GetChart(DataSource input) =>
            new Charts
            {
               piechart=gc.Graph_Maker(input),
               linechart= gc.Graph_Maker(input),
               barchart= gc.Graph_Maker(input),
               doublechart= gc.Graph_Maker(input),
               radarchart=gc.Graph_Maker(input),
               doughnutchart=gc.Graph_Maker(input),
               polarchart= gc.Graph_Maker(input),
               scatterchart = gc.Graph_Maker(input),
               bubblechart = gc.Graph_Maker(input)

            };
    }
}
