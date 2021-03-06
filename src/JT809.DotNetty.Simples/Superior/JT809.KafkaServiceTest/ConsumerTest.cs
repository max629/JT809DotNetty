﻿using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using JT809.Protocol.Enums;
using JT809.Protocol.Extensions;
using System.Threading;

namespace JT809.KafkaServiceTest
{
    public  class ConsumerTest: TestConsumerBase
    {
        [Fact]
        public void Test1()
        {
            ConsumerTestService consumerTestService = ServiceProvider.GetRequiredService<ConsumerTestService>();
            consumerTestService.GpsConsumer.OnMessage((Message)=> 
            {
                Assert.Equal(JT809SubBusinessType.实时上传车辆定位信息.ToValueString(), Message.MsgId);
                Assert.Equal("粤A23456", Message.Data.Vno);
                Assert.Equal(2, Message.Data.VColor);
                Assert.Equal("smallchi", Message.Data.FromChannel);
            });
            consumerTestService.GpsConsumer.Subscribe();

            Thread.Sleep(100000);
        }

        [Fact]
        public void Test2()
        {
            ConsumerTestService consumerTestService = ServiceProvider.GetRequiredService<ConsumerTestService>();
            consumerTestService.SameConsumer.OnMessage((Message) =>
            {
                Assert.Equal(JT809SubBusinessType.None.ToValueString(), Message.MsgId);
                Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, Message.Data);
            });
            consumerTestService.SameConsumer.Subscribe();

            Thread.Sleep(100000);
        }
    }
}
