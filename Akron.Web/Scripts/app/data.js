var data = null;
var ndx = null;
var mainDim = null;
var mainGroup = null;
var processedData = null;
var chart = dc.lineChart("#chart");
var barChart = dc.barChart("#barChart");
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
        $('#recordCount').text(r.RecordCount);
        $('#averageBasePay').text(r.TotalAverage);
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

        $('#orgTypeCount').text(dimensionKeys.length);
        $('#yearCount').text(processed.length);
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

        allGroup = mainDim.group().reduce(function(p, v) {

            ++p.count;

            var found = false;
            for (var i=0; i < p.values.length;i++) {
                if (p.values[i].key == v.key)
                    found = true;
            }
            if (!found) {
                

                var ct = 0;
                var tot = 0;
                for (var i = 0; i < dimensionKeys.length;i++) {
                    if (v[dimensionKeys[i]] != null) {
                        ++ct;
                        tot += v[dimensionKeys[i]];
                    }
                }
                p.values.push({ key: v.key, value: Math.floor(tot / ct) });
            }
           

            
            return p;
        }, function(p, v) {
            

        }, function() {
            return { count: 0, values:[] };
        });
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
            return v;
        });
        chart.stackLayout().out(function (d, y0, y) {
            d.y0 = 0;
            d.y = y;
        });


        barChart
            .width(300)
            .height(200)
            .x(d3.scale.linear().domain([2006, 2012]))
            .y(d3.scale.linear().domain([20000, 90000]))
            .gap(1)
            .brushOn(false)
            .dimension(mainDim)
            .group(allGroup)
            .keyAccessor(function(d) {
                return d.value.values[0].key;
            })
            .valueAccessor(function(d) {
                return d.value.values[0].value;
            });
        barChart.yAxis().tickFormat(function (v) {
            return v / 1000;
        });
        barChart.xAxis().tickFormat(function(d) {
            return d.toString();
        });
        barChart.xAxis().ticks(7);
            dc.renderAll();

    });
}

$(function () {
    init();
});