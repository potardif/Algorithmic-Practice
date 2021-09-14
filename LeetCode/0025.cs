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
	public ListNode ReverseKGroup(ListNode head, int k) {
		if (k == 1) {
			return head;
		}

		ListNode prev = null;
		ListNode cur = head;

		while (true) {
			if (!LongEnough(cur, k)) {
				return head;
			}

			ListNode newHead = cur;
			ListNode newTail = cur;

			ListNode left = cur;
			cur = cur.next;
			for (int i = 0; i < k - 1; ++i) {
				ListNode next = cur.next;

				newHead = cur;
				cur.next = left;

				left = cur;
				cur = next;
			}
			newTail.next = cur;

			if (prev == null) {
				head = newHead;
			} else {
				prev.next = newHead;
			}
			prev = newTail;
		}
	}

	private bool LongEnough(ListNode head, int k) {
		int i = 0;
		for (; head != null && i < k; ++i) {
			head = head.next;
		}
		return i == k;
	}
}
