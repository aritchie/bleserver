//using System;


//namespace Acr.Beacons
//{
//    public class BeaconAdvertiserImpl : IBeaconAdvertiser
//    {
//        public void Start(BeaconConfig config)
//        {
//            throw new NotImplementedException();
//        }


//        public void Stop()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
////        public override void StartAdvertising(BeaconRegion region)
////        {
////            // TODO: need local name
////            // TODO: all parts of the region need to be set
////            using (var ms = new MemoryStream())
////            {
////                var buffer = new Guid(region.Uuid).ToByteArray();
////                ms.Write(buffer, 0, buffer.Length);

////                buffer = BitConverter.GetBytes(region.Major.Value);
////                ms.Write(buffer, 0, buffer.Length);

////                BitConverter.GetBytes(region.Minor.Value);
////                ms.Write(buffer, 0, buffer.Length);

////                buffer = ms.ToArray();
////                // TODO: can't set manufacturer data
////                //this.adapter.LeGattServer.StartAdvertising(x => x.);
////            }
////        }


////        public override void StopAdvertising()
////        {
////            //this.adapter.LeGattServer.StopAdvertising();
////        }