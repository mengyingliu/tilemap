# 地理空间切片数据动态更新方法研究与实现 
特别说明：本产品只为沟通和学习交流使用，任何形式的商业用途都是不被允许的，对于产品可能带来的法律问题，开发者保留最终的解释权。
#项目简介
本文基于瓦片地图技术的原理与方法，利用AE和C#生成地图切片方案，生成ArcGIS10提出的松散型（Exploded）和紧凑型（Compacted）切片数据文件，，基于devexpress设计并搭建了地图切片数据管理系统。该系统主要为地图进行全局切片和动态更新提供思路和方法。
##系统主要功能
![image](https://raw.githubusercontent.com/mengyingliu/tilemap/master/pics/%E7%B3%BB%E7%BB%9F%E5%8A%9F%E8%83%BD.png “github”);
##切片方案生成
