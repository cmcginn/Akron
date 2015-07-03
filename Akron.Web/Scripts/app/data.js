var data = null;
var ndx = null;
var mainDim = null;
var mainGroup = null;
var processedData = null;
var chart = dc.seriesChart("#chart");
var barChart = dc.barChart("#barChart");
var bubbleChart = dc.bubbleChart('#bubbleChart');

var allGroup = null;
var allDim = null;
var group1 = null;
var group2 = null;
var group3 = null;
var processed = [];
var dimensionKeys = [];
var processedData = [];
var groups = [];
var allOrgGroup = null;
var mainFilter = null;
var orgDim = null;
var orgGroup = 0;
var yearDim = null;
var yearGroup = null;
var ordinals;
var countNdx = null;
var countOrgDim = null;
var countOrgGroup = null;
function init() {
    
    d3.json(root + 'api/Incumbent', function (r) {
        data = r;
        $('#recordCount').text(r.RecordCount);
        $('#averageBasePay').text(r.TotalAverage);
        var orgTypes = [];
        data.BasePayByYearAndDimension.forEach(function(d) {
            var org = d[0]._value[1]._value;
            if (orgTypes.indexOf(org) < 0)
                orgTypes.push(org);
        });
        $('#orgTypeCount').text(orgTypes.length);
        ndx = crossfilter(data.BasePayByYearAndDimension);
        countNdx = crossfilter(data.CountByDimension);

        mainDim = ndx.dimension(function (d) {
            return [d[0]._value[0]._value,d[0]._value[1]._value];
        });
        yearDim = ndx.dimension(function (d) {
            return d[0]._value[0]._value;
        });

        countOrgDim = countNdx.dimension(function(d) {
            return d[0]._value;
        });
        mainGroup = mainDim.group().reduceSum(function(d) {
            return d[1]._value;
        });
        countOrgGroup = countOrgDim.group().reduce(function(p, v) {
            p.count = v[1]._value;
            return p;
        }, function(p, v) {
            
        }, function() {
            return { count: 0 };
        });
        yearGroup = yearDim.group().reduce(function(p, v) {
            ++p.count;
            p.total += v[1]._value;
            p.average = Math.floor(p.total / p.count);
            return p;
        }, function(p, v) {
            
        }, function() {
            return {count:0,average:0,total:0};
        });

        $('#yearCount').text(yearGroup.all().length);
        chart
            .dimension(mainDim)
            .width(600)
            .height(400)
            .group(mainGroup)
            .colors(d3.scale.category20b())
            .brushOn(false)
            .legend(dc.legend().x(100).y(0).itemHeight(8).gap(5).autoItemWidth(true))
            .y(d3.scale.linear().domain([40000, 130000]))
            .x(d3.scale.linear().domain([2006, 2012]))
            .keyAccessor(function(d) {
                return d.key[0];
            })
            .valueAccessor(function(d) {
                return d.value;
            })
            .seriesAccessor(function(d) {
            return d.key[1];
        });
        chart.yAxis().tickFormat(function (v) {
            return v / 1000;
        });
        chart.xAxis().ticks(7);
        chart.xAxis().tickFormat(function (v) {
            return v;
        });
      

        barChart
            .width(300)
            .height(200)
            .x(d3.scale.linear().domain([2006, 2012]))
            .y(d3.scale.linear().domain([20000, 90000]))
            .gap(1)
            .brushOn(false)
            .dimension(yearDim)
            .group(yearGroup)
            .keyAccessor(function(d) {
                return d.key;
            })
            .valueAccessor(function(d) {
            return d.value.average;
        });
        barChart.yAxis().tickFormat(function (v) {
            return v / 1000;
        });
        barChart.xAxis().tickFormat(function(d) {
            return d.toString();
        });
        barChart.xAxis().ticks(7);
        var ordinals = [];
        var all = countOrgGroup.all();
        all.forEach(function (d) {
            ordinals.push(d.key);
        });
        var maximum = d3.sum(all, function (d) { return d.value.count; });
        bubbleChart
        .width(990)
                    .height(250)
                    .transitionDuration(1500)
                    .margins({ top: 30, right: 50, bottom: 30, left: 40 })
                    .dimension(countOrgDim)

                    .group(countOrgGroup)
                    .colorDomain([-0.0, 100.1])
                    .colors(d3.scale.category20b())
                    .yAxisPadding(20)
                    .colorDomain([0, 100])
                    .colorAccessor(function (d) {
                        return Math.round((d.value.count / maximum) * 100);
                    })
                    .keyAccessor(function (p) {
                        return p.key;
                    })
                    .valueAccessor(function (p) {
                        return Math.round(((p.value.count / maximum) * 100), 4);
                    })
                    .radiusValueAccessor(function (p) {
                        return (p.value.count / maximum) * 40;
                    })
                    .maxBubbleRelativeSize(0.5)
                    .x(d3.scale.ordinal().domain(ordinals))
                    .xUnits(dc.units.ordinal)
                    .y(d3.scale.linear().domain([-5, 30]))
                    .r(d3.scale.linear().domain([0, 100]))
                    .xAxisPadding(900)
                    .title(function (d) {
                        return d.key + ':' + Math.round(((d.value.count / maximum) * 100), 2) + '%';
                    })
                    .elasticX(true)
                    .yAxisPadding(100)
                    .xAxisPadding(500)
                    .renderHorizontalGridLines(true)
                    .renderVerticalGridLines(true)
                    .label(function (d) {
                        var lbl = d.key;
                        if (d.key.length > 12)
                            lbl = d.key.substring(0, 10) + "...";
                        return lbl;
                    });

        bubbleChart.yAxis().tickFormat(function (v) {
            return v + '%';
        });

        yearGroup.all().forEach(function(val) {
            var val = $('<tr><td>' + val.key + '</td><td>' + val.value.average + '</td></tr>');

            $('#totalsTable tbody').append(val);
        });

            dc.renderAll();

    });
}

$(function () {
    init();
});