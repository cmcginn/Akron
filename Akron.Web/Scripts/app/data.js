var data = null;
var ndx = null;
var mainDim = null;
var mainGroup = null;
var processedData = null;
var chart = dc.seriesChart("#chart");
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
var allOrgGroup = null;
var mainFilter = null;
var orgDim = null;
var orgGroup = 0;
var yearDim = null;
var yearGroup = null;
var ordinals;
function init() {
    
    d3.json(root + 'api/Incumbent', function (r) {
        data = r;
        $('#recordCount').text(r.RecordCount);
        $('#averageBasePay').text(r.TotalAverage);
        var orgTypes = [];
        data.BasePayByYearAndOrgType.forEach(function(d) {
            var org = d[0]._value[1]._value;
            if (orgTypes.indexOf(org) < 0)
                orgTypes.push(org);
        });
        $('#orgTypeCount').text(orgTypes.length);
        ndx = crossfilter(data.BasePayByYearAndOrgType);
        mainDim = ndx.dimension(function (d) {
            return [d[0]._value[0]._value,d[0]._value[1]._value];
        });
        yearDim = ndx.dimension(function (d) {
            return d[0]._value[0]._value;
        });


        mainGroup = mainDim.group().reduceSum(function(d) {
            return d[1]._value;
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

       
               
           

            dc.renderAll();

    });
}

$(function () {
    init();
});