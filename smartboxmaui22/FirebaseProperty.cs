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
        public string PhoneNumber { get; set; }

        [FirestoreProperty]
        //필드 이름
        public string BoxNumber { get; set; }

        [FirestoreProperty]
        //필드 이름
        public string Password { get; set; }
    }
}
