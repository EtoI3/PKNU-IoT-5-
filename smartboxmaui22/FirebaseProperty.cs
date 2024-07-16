using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartboxmaui2
{
    [FirestoreData]
    public class FirebaseProperty
    {
        [FirestoreProperty]
        //필드 이름
        public string phone_number { get; set; }

        [FirestoreProperty]
        //필드 이름
        public string box_number { get; set; }

        [FirestoreProperty]
        //필드 이름
        public int password { get; set; }
    }
}
