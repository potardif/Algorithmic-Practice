/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     public int val;
 *     public ListNode next;
 *     public ListNode(int val=0, ListNode next=null) {
 *         this.val = val;
 *         this.next = next;
 *     }
 * }
 */
public class Solution {
	public ListNode MergeKLists(ListNode[] lists) {
		if (lists.Length == 0)
			return null;

		int n = lists.Length;
		while (n > 1) {
			int i = 0;
			for (; i + 1 < n; i += 2) {
				lists[i / 2] = Merge2Lists(lists[i], lists[i + 1]);
			}

			if (i < n) {
				lists[i / 2] = lists[i];
			}

			n = n / 2 + n % 2;
		}

		return lists[0];
	}

	private ListNode Merge2Lists(ListNode list1, ListNode list2) {
		ListNode head = null;
		ListNode prev = null;

		while (true) {
			ListNode cur;
			switch ((list1, list2)) {
				case (null, null):
					if (prev != null) {
						prev.next = null;
					}
					return head;
				case (null, _):
					if (prev == null) {
						return list2;
					} else {
						prev.next = list2;
						return head;
					}
				case (_, null):
					if (prev == null) {
						return list1;
					} else {
						prev.next = list1;
						return head;
					}
				default:
					if (list1.val < list2.val) {
						cur = list1;
						list1 = list1.next;
					} else {
						cur = list2;
						list2 = list2.next;
					}
					break;
			}

			if (prev == null) {
				head = cur;
			} else {
				prev.next = cur;
			}
			prev = cur;
		}
	}
}
