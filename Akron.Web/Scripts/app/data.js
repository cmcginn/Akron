var data = null;
var ndx = null;
var mainDim = null;
var mainGroup = null;
var processedData = null;
var chart = dc.lineChart("#chart");
var allGroup = null;
var allDim = null;
var group1 = null;
var group2 = null;
var group3 = null;
var processed = [];
var dimensionKeys = [];
var processedData = [];
var groups = [];
var mainFilter = null;

function init() {
    
    d3.json(root + 'api/Incumbent', function (r) {
        data = r;

        processed = d3.nest()
            .key(function (d) { return d[0]._value[1]._value; })
            .key(function (d) { return d[0]._value[0]._value; })
            .entries(data.BasePayByYearAndOrgType);
        processed.forEach(function (d) {
            var item = { key: +d.key };

            d.values.forEach(function (n) {
                if (item[n.key] == null) {

                    item[n.key] = Math.floor(n.values[0][1]._value);

                }

                if (dimensionKeys.indexOf(n.key) < 0) {
                    if (!isNaN(item[n.key]))
                        dimensionKeys.push(n.key);
                }


            });
            processedData.push(item);
        });


        ndx = crossfilter(processedData);
        mainDim = ndx.dimension(function (d) { return d.key; });

        for (var i = 0; i < dimensionKeys.length; i++) {

            var group = mainDim.group().reduceSum(function (d) {

                var am = isNaN(d[dimensionKeys[i]]) ? -1 : d[dimensionKeys[i]];
                return am;
            });
            var zz = group.all();

            groups.push(group);
        }

        chart
            .dimension(mainDim)
            .width(600)
            .height(400)
            .group(groups[0], dimensionKeys[0])
            .colors(d3.scale.category20b())
            .brushOn(false)
            .legend(dc.legend().x(100).y(0).itemHeight(8).gap(5).autoItemWidth(true))
            .renderDataPoints(true)
            .y(d3.scale.linear().domain([40000, 130000]))
            .x(d3.scale.linear().domain([2006, 2012]));

        for (var i = 1; i < groups.length; i++) {

            chart.stack(groups[i], dimensionKeys[i]);
        }
        chart.yAxis().tickFormat(function (v) {
            return v / 1000;
        });
        chart.xAxis().tickFormat(function (v) {
            return Math.floor(v).toString();
        });
        chart.stackLayout().out(function (d, y0, y) {
            d.y0 = 0;
            d.y = y;
        });

        dc.renderAll();

    });
}

$(function () {
    init();
});